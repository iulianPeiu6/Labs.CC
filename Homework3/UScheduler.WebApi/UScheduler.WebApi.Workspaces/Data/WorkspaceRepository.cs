using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System;
using LiteDB;
using UScheduler.WebApi.Workspaces.Data.Entities;
using UScheduler.WebApi.Workspaces.Interfaces;

namespace UScheduler.WebApi.Workspaces.Data
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly ILiteDatabase database;

        private readonly ILiteCollection<Workspace> collection;

        public WorkspaceRepository()
        {

            var databasePath = Path.Combine("Workspace.db");

            var connectionString = @$"Filename={databasePath}; Connection=Shared;";

            database = new LiteDatabase(connectionString);

            collection = database.GetCollection<Workspace>($"{ typeof(Workspace).Name }s");
        }

        public Workspace Add(Workspace entity)
        {
            return collection.FindById(collection.Insert(entity));
        }

        public IEnumerable<Workspace> GetAll()
        {
            return collection.FindAll();
        }

        public Workspace GetById(Guid id)
        {
            return collection.FindById(id);
        }

        public Workspace Update(Guid id, Workspace entity)
        {
            collection.Update(id, entity);

            var result = collection.FindById(id);

            if (result == null)
            {
                throw new ArgumentNullException("Entity not found");
            }

            return result;
        }

        public bool Delete(Guid id)
        {
            return collection.Delete(id);
        }

        public ILiteQueryable<Workspace> Query(Expression<Func<Workspace, bool>> expression)
        {
            return collection.Query().Where(expression);
        }

        public ILiteQueryable<Workspace> Query()
        {
            return collection.Query();
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
