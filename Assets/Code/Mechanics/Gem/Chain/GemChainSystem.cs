using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PMT
{
    internal class GemChainSystem : IGemChainSystem
    {
        private List<GemChain> _chains;

        public GemChainSystem()
        {
            _chains = new List<GemChain>();

            EventBus<MatchEvent>.Subscribe(OnMatchEvent);
        }

        public void OnMatchEvent(MatchEvent matchEvent)
        {
            if (matchEvent.Match)
                Match(matchEvent.Sender, matchEvent.Other);
            else
                Match(matchEvent.Sender, matchEvent.Other);
        }

        public void Match(Gem gem, Gem connection)
        {
            var containingChains = _chains.Where(chain => chain.ContainsAny(gem, connection)).ToList();

            if (containingChains.Count == 0)
            {
                var newChain = new GemChain();
                newChain.AddConnection(gem, connection);
                _chains.Add(newChain);
            }
            else if (containingChains.Count == 1)
            {
                containingChains[0].AddConnection(gem, connection);
            }
            else
            {
                var mergedChain = new GemChain();

                foreach (var chain in containingChains)
                {
                    mergedChain.Merge(chain);
                    _chains.Remove(chain);
                }

                mergedChain.AddConnection(gem, connection);
                _chains.Add(mergedChain);
            }
        }

        public void Dismatch(Gem gem, Gem connection)
        {
            var containingChain = _chains.FirstOrDefault(chain => chain.ContainsBoth(gem, connection));

            if (containingChain == null)
                return;

            containingChain.RemoveConnection(gem, connection);

            var separatedChains = containingChain.SplitIfDisconnected();

            if (separatedChains != null)
            {
                _chains.Remove(containingChain);
                _chains.AddRange(separatedChains);
            }

            _chains.RemoveAll(chain => chain.IsEmpty);
        }

        public void Dispose() { }

        private class GemChain
        {
            private readonly Dictionary<Gem, HashSet<Gem>> _connections = new();

            public int GemCount => _connections.Count;
            public bool IsEmpty => _connections.Count == 0;

            public void AddConnection(Gem gem1, Gem gem2)
            {
                AddOneWayConnection(gem1, gem2);
                AddOneWayConnection(gem2, gem1);
            }

            private void AddOneWayConnection(Gem from, Gem to)
            {
                if (!_connections.TryGetValue(from, out var connections))
                {
                    connections = new HashSet<Gem>();
                    _connections[from] = connections;
                }
                connections.Add(to);
            }

            public void RemoveConnection(Gem gem1, Gem gem2)
            {
                RemoveOneWayConnection(gem1, gem2);
                RemoveOneWayConnection(gem2, gem1);
            }

            private void RemoveOneWayConnection(Gem from, Gem to)
            {
                if (_connections.TryGetValue(from, out var connections))
                {
                    connections.Remove(to);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(from);
                    }
                }
            }

            public bool ContainsAny(params Gem[] gems)
            {
                return gems.Any(gem => _connections.ContainsKey(gem));
            }

            public bool ContainsBoth(Gem gem1, Gem gem2)
            {
                return _connections.ContainsKey(gem1) && _connections.ContainsKey(gem2) &&
                       _connections[gem1].Contains(gem2) && _connections[gem2].Contains(gem1);
            }

            public void Merge(GemChain other)
            {
                foreach (var pair in other._connections)
                {
                    if (!_connections.TryGetValue(pair.Key, out var existingConnections))
                    {
                        _connections[pair.Key] = new HashSet<Gem>(pair.Value);
                    }
                    else
                    {
                        existingConnections.UnionWith(pair.Value);
                    }
                }
            }

            public List<GemChain> SplitIfDisconnected()
            {
                if (_connections.Count == 0)
                    return null;

                var visited = new HashSet<Gem>();
                var components = new List<HashSet<Gem>>();

                foreach (var gem in _connections.Keys)
                {
                    if (!visited.Contains(gem))
                    {
                        var component = new HashSet<Gem>();
                        Traverse(gem, visited, component);
                        components.Add(component);
                    }
                }

                if (components.Count == 1)
                    return null;

                var result = new List<GemChain>();
                foreach (var component in components)
                {
                    var newChain = new GemChain();
                    foreach (var gem in component)
                    {
                        foreach (var connectedGem in _connections[gem])
                        {
                            if (component.Contains(connectedGem))
                            {
                                newChain.AddConnection(gem, connectedGem);
                            }
                        }
                    }
                    result.Add(newChain);
                }

                return result;
            }

            private void Traverse(Gem current, HashSet<Gem> visited, HashSet<Gem> component)
            {
                var stack = new Stack<Gem>();
                stack.Push(current);

                while (stack.Count > 0)
                {
                    var gem = stack.Pop();
                    if (visited.Add(gem))
                    {
                        component.Add(gem);
                        foreach (var neighbor in _connections[gem])
                        {
                            if (!visited.Contains(neighbor))
                            {
                                stack.Push(neighbor);
                            }
                        }
                    }
                }
            }
        }
    }
}