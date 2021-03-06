﻿using System;
using System.IO;
using ByteDev.Testing.TestBuilders.FileSystem;

namespace ByteDev.Csv.IntTests
{
    public class IoBase
    {
        private readonly string _intTestsRootDirectory;

        public IoBase(string intTestsRootDirectory)
        {
            _intTestsRootDirectory = intTestsRootDirectory;
        }

        /// <summary>
        /// Working directory to be used by the method under test
        /// </summary>
        protected string WorkingDir { get; private set; }

        /// <summary>
        /// Set the method under test's working directory for IO
        /// integration tests
        /// </summary>
        /// <param name="type">Type under test</param>
        /// <param name="methodName">FullName of method under test</param>
        protected void SetWorkingDir(Type type, string methodName)
        {
            WorkingDir = Path.Combine(_intTestsRootDirectory, type.Name, GetShortMethodName(methodName));
        }

        protected void EmptyWorkingDir()
        {
            if(string.IsNullOrEmpty(WorkingDir))
            {
                throw new InvalidOperationException("Working directory has not been set");                
            }
            CreateWorkingDir().Empty();
        }

        protected DirectoryInfo CreateWorkingDir()
        {
            return DirectoryTestBuilder.InFileSystem.WithPath(WorkingDir).Build();
        }

        protected string GetAbsolutePathFor(string path)
        {
            return Path.Combine(WorkingDir, path);
        }

        private static string GetShortMethodName(string methodName)
        {
            var plusPos = methodName.LastIndexOf("+", StringComparison.Ordinal);

            if (plusPos < 0)
            {
                return methodName;
            }
            return methodName.Substring(plusPos + 1);
        }
    }
}
