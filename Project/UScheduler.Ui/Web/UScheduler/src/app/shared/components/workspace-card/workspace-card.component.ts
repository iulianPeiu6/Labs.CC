import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DxButtonModule, DxFormModule, DxPopupModule } from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import notify from 'devextreme/ui/notify';
import { Workspace, WorkspacesService } from '../../services/workspaces.service';

@Component({
  selector: 'app-workspace-card',
  templateUrl: './workspace-card.component.html',
  styleUrls: ['./workspace-card.component.scss']
})
export class WorkspaceCardComponent implements OnInit {
  workspace: Workspace | null;
  accessLevels: DataSource;
  updateWorkspacePopupIsVisible: boolean;

  constructor(private workspaceService: WorkspacesService, private router: Router) { 
    this.workspace = new Workspace();
    this.updateWorkspacePopupIsVisible = false;
    this.accessLevels = new DataSource({
      store: {
          type: "array",
          data: [ 'Private', 'Public' ]
      }
    });
  }

  ngOnInit(): void {
  }

  async toggleEditWorkspace() {
    this.updateWorkspacePopupIsVisible = true;
  }

  async deleteWorkspace() {
    try {
      await this.workspaceService.delete(this.workspace?.id!);
      notify('Workspace deleted successuflly', 'success');
      this.workspace = null;
    }
    catch (e){
      console.log(e);
      notify('Error when deleting workspaces. Check the console for details', 'error');
    }
  }

  async updateWorkspace(e: any) {
    e.preventDefault();
    try {
      this.workspace = await this.workspaceService.update(this.workspace!);
      notify('Workspace updated successuflly', 'success');
      this.updateWorkspacePopupIsVisible = false;
    }
    catch (e){
      console.log(e);
      notify('Error when updating workspaces. Check the console for details', 'error');
    }
  }

  openWorkspace() {
    this.router.navigateByUrl(`/workspaces/${this.workspace?.id}/boards`);
  }
}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule,
    DxPopupModule,
    DxFormModule,
    DxButtonModule
  ],
  declarations: [ WorkspaceCardComponent ],
  exports: [ WorkspaceCardComponent ]
})
export class WorkspaceCardModule { }
