import { Component, NgModule, OnInit } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { Workspace, WorkspacesService } from '../../shared/services/workspaces.service';
import notify from "devextreme/ui/notify";

@Component({
  selector: 'app-create-workspace',
  templateUrl: './create-workspace.component.html',
  styleUrls: ['./create-workspace.component.scss']
})
export class CreateWorkspaceComponent{
  workspace: Workspace;
  accessLevels: DataSource;
  workspaceTemplates: DataSource;

  constructor(private workspacesService: WorkspacesService) { 
    this.workspace = new Workspace();
    this.accessLevels = new DataSource({
      store: {
          type: "array",
          data: [ 'Private', 'Public' ]
      }
    });
    
    this.workspaceTemplates = new DataSource({
      store: {
          type: "array",
          data: [ 'Empty', 'Basic' ]
      }
    });
  }

  createWorkspace(e: any) {
    e.preventDefault();
    console.log(`Creating workspace:\'${this.workspace.title}\'`);
    this.workspacesService
      .createWorkspace(this.workspace)
      .then(createdWorkspace => {
        notify(`Workspace '${createdWorkspace.title}' was created.`, "success", 3000);
      })
      .catch(error => {
        notify('Workspace could not be created.', "error", 3000);
        console.log(error);
      });
  }
}
