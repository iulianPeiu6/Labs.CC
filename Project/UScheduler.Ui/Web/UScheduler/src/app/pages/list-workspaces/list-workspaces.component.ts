import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import DataSource from 'devextreme/data/data_source';
import notify from 'devextreme/ui/notify';
import { WorkspaceCardComponent } from 'src/app/shared/components/workspace-card/workspace-card.component';
import { Workspace, WorkspacesService } from 'src/app/shared/services/workspaces.service';

@Component({
  selector: 'app-list-workspaces',
  templateUrl: './list-workspaces.component.html',
  styleUrls: ['./list-workspaces.component.scss']
})
export class ListWorkspacesComponent implements OnInit {
  @ViewChild('workspacesCards', { read: ViewContainerRef }) workspacesCards!: ViewContainerRef;
  workspace: Workspace;
  accessLevels: DataSource;
  workspaceTemplates: DataSource;
  createWorkspacePopupIsVisible: boolean;
  filter: string;

  constructor(
    private route: ActivatedRoute,
    private workspacesService: WorkspacesService) {

    console.log("ListWorkspacesComponent");
    this.filter = window.location.href.split('/').pop()!;
    this.workspace = new Workspace();
    this.createWorkspacePopupIsVisible = false;
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

  async ngOnInit(): Promise<void> {
    let workspaces = await this.workspacesService.filter(this.filter);
    workspaces.forEach(workspace => {
      var component = this.workspacesCards.createComponent(WorkspaceCardComponent);
      component.instance.workspace = workspace;
    });
  }

  toggleCreateWorkspacePopup() {
    this.createWorkspacePopupIsVisible = true;
  }

  createWorkspace(e: any) {
    e.preventDefault();
    console.log(`Creating workspace:\'${this.workspace.title}\'`);
    this.workspacesService
      .create(this.workspace)
      .then(createdWorkspace => {
        notify(`Workspace '${createdWorkspace.title}' was created.`, "success", 3000);
        var component = this.workspacesCards.createComponent(WorkspaceCardComponent);
        component.instance.workspace = createdWorkspace;
        this.createWorkspacePopupIsVisible = false;
      })
      .catch(error => {
        notify('Workspace could not be created.', "error", 3000);
        console.log(error);
      });
  }
}
