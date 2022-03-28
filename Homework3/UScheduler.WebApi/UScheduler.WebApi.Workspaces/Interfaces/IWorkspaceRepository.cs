using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UScheduler.WebApi.Workspaces.Data.Entities;

namespace UScheduler.WebApi.Workspaces.Interfaces
{
    public interface IWorkspaceRepository
    {
        Workspace Add(Workspace entity);
        bool Delete(Guid id);
        void Dispose();
        IEnumerable<Workspace> GetAll();
        Workspace GetById(Guid id);
        ILiteQueryable<Workspace> Query();
        ILiteQueryable<Workspace> Query(Expression<Func<Workspace, bool>> expression);
        Workspace Update(Guid id, Workspace entity);
    }
}