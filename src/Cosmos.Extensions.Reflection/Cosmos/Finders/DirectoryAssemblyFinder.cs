﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Cosmos.Finders
{
    /// <summary>
    /// Directory assembly finder
    /// </summary>
    public class DirectoryAssemblyFinder : IAssemblyFinder
    {
        private static readonly ConcurrentDictionary<string, Assembly[]> AssemblyCacheDict;
        private readonly string _path;

        static DirectoryAssemblyFinder()
        {
            AssemblyCacheDict = new ConcurrentDictionary<string, Assembly[]>();
        }

        public DirectoryAssemblyFinder(string path)
        {
            _path = path;
        }

        public Assembly[] Find(Func<Assembly, bool> predicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(predicate).ToArray();
        }

        public Assembly[] FindAll(bool fromCache = false)
        {
            if (fromCache && AssemblyCacheDict.ContainsKey(_path))
                return AssemblyCacheDict[_path];

            var files = Directory.GetFiles(_path, "*.dll", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(_path, "*.exe", SearchOption.TopDirectoryOnly))
                .ToArray();
            
            var assemblies = files.Select(Assembly.LoadFrom).Distinct().ToArray();
            
            AssemblyCacheDict[_path] = assemblies;
            
            return assemblies;
        }
    }
}