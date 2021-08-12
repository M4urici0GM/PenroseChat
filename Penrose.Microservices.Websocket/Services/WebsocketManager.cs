using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.Extensions.Logging;
using Penrose.Core.Entities;
using Penrose.Core.Exceptions;

namespace Penrose.Microservices.Websocket.Services
{

    public class WebsocketConnection
    {
        public Guid Id { get; }
        public WebSocket Connection { get; }
        public User User { get; }
        public DateTime ConnectedAt { get; }

        public WebsocketConnection(WebSocket connection)
        {
            Id = Guid.NewGuid();
            Connection = connection;
            ConnectedAt = DateTime.UtcNow;
        }
        
        public WebsocketConnection(WebSocket connection, User user)
        {
            Id = Guid.NewGuid();
            Connection = connection;
            User = user;
            ConnectedAt = DateTime.UtcNow;
        }
    }

    public interface IWebsocketManager
    {
        WebsocketConnection GetConnection(Guid connectionId);
        bool UserHasConnection(Guid userId);
        bool HasConnection(Guid connectionId);
        WebsocketConnection InsertConnection(WebSocket connection);
        void RemoveConnetion(Guid connectionId);
    }
    
    public class WebsocketManager : IWebsocketManager
    {
        private readonly ILogger<WebsocketManager> _logger;
        private readonly ConcurrentDictionary<Guid, WebsocketConnection> _websocketConnections;
        private readonly ConcurrentDictionary<Guid, IEnumerable<Guid>> _userWebsocketIndexes;

        public WebsocketManager(ILogger<WebsocketManager> logger)
        {
            _logger = logger;
            _websocketConnections = new ConcurrentDictionary<Guid, WebsocketConnection>();
            _userWebsocketIndexes = new ConcurrentDictionary<Guid, IEnumerable<Guid>>();
        }

        public WebsocketConnection GetConnection(Guid connectionId)
        {
            if (HasConnection(connectionId))
                throw new EntityNotFoundException(nameof(WebsocketConnection), connectionId);

            return _websocketConnections.GetValueOrDefault(connectionId);
        }

        public bool UserHasConnection(Guid userId)
        {
            return _userWebsocketIndexes.ContainsKey(userId);
        }

        public bool HasConnection(Guid connectionId)
        {
            return _websocketConnections.ContainsKey(connectionId);
        }

        public WebsocketConnection InsertConnection(WebSocket connection)
        {
            WebsocketConnection websocketConnection = new(connection);
            bool hasInserted = _websocketConnections.TryAdd(websocketConnection.Id, websocketConnection);
            if (!hasInserted)
                throw new Exception("Failed to add new connection to the manager.");

            _logger.LogDebug("New client connected: {ClientId}", websocketConnection.Id);
            return websocketConnection;
        }

        private void AddConnectionToUserIndex(Guid userId, WebsocketConnection connection)
        {
            List<Guid> userConnections = GetUserConnections(userId).ToList();
            userConnections.Add(connection.Id);
            
            UpdateUserIndex(userId, userConnections);
        }

        private IEnumerable<Guid> GetUserConnections(Guid userId)
        {
            bool hasValues = _userWebsocketIndexes.TryGetValue(userId, out IEnumerable<Guid> userConnectionIds);
            return !hasValues
                ? new List<Guid>()
                : userConnectionIds;
        }

        private void UpdateUserIndex(Guid userId, IEnumerable<Guid> newConnections)
        {
            List<Guid> userConnections = GetUserConnections(userId).ToList();

            bool hasIndex = HasUserIndex(userId);
            if (!hasIndex)
            {
                _ = InsertUserIndex(userId, newConnections);
                return;
            }

            bool hasUpdated = _userWebsocketIndexes.TryUpdate(userId, newConnections, userConnections);
            if (!hasUpdated)
                throw new Exception("Failed to update user index.");

            _logger.LogDebug(
                "UserIndex {UserId} updated, total connections: {Connections}",
                userId,
                userConnections.Count);
        }
        
        private bool HasUserIndex(Guid userId)
        {
            return _userWebsocketIndexes.ContainsKey(userId);
        }

        private Guid InsertUserIndex(Guid userId, IEnumerable<Guid> userConnections)
        {
            bool hasInserted = _userWebsocketIndexes.TryAdd(userId, userConnections);
            if (!hasInserted)
                throw new Exception("Failed to add new user index");

            return userId;
        }
        
        private void RemoveUserIndex(Guid userId)
        {
            List<Guid> userConnections = GetUserConnections(userId).ToList();
            foreach (Guid connectionId in userConnections)
                RemoveConnetion(connectionId);

            bool hasRemoved = _userWebsocketIndexes.TryRemove(userId, out var _);
            if (!hasRemoved)
                throw new Exception("Failed to remove user index from list");
        }

        public void RemoveConnetion(Guid connectionId)
        {
            bool hasRemoved = _websocketConnections.TryRemove(connectionId, out var _);
            if (!hasRemoved)
                throw new Exception("Failed to remove connection from list.");
        }
    }
}