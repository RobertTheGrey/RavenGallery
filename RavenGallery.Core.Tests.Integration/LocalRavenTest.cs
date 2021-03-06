﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Raven.Client.Document;
using Raven.Database;
using NUnit.Framework;
using System.Threading;
using Raven.Client.Indexes;
using RavenGallery.Core.Indexes;
using Raven.Database.Extensions;
using Raven.Client.Client;

namespace RavenGallery.Core.Tests.Integration
{
    public class LocalRavenTest
    {
        private EmbeddableDocumentStore store;
        public EmbeddableDocumentStore Store { get { return store; } }

        [SetUp]
        public void CreateStore()
        {
            store = new EmbeddableDocumentStore
            {
                Configuration = new RavenConfiguration
                {
                    RunInMemory = true
                }
            };
            store.Initialize();
            IndexCreation.CreateIndexes(typeof(ImageTags_GroupByTagName).Assembly, store);
        }

        public void WaitForIndexing()
        {
            while (store.DocumentDatabase.Statistics.StaleIndexes.Length > 0)
            {
                Thread.Sleep(100);
            }
        }

        [TearDown]
        public void DestroyStore()
        {
            store.Dispose();
        }
    }
}
