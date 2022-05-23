import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { DxButtonModule } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { Workspace, WorkspacesService } from '../../services/workspaces.service';

@Component({
  selector: 'app-workspace-card',
  templateUrl: './workspace-card.component.html',
  styleUrls: ['./workspace-card.component.scss']
})
export class WorkspaceCardComponent implements OnInit {
  workspace: Workspace | null;

  constructor(private workspaceService: WorkspacesService) { 
    this.workspace = new Workspace();
  }

  ngOnInit(): void {
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

}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule
  ],
  declarations: [ WorkspaceCardComponent ],
  exports: [ WorkspaceCardComponent ]
})
export class WorkspaceCardModule { }
