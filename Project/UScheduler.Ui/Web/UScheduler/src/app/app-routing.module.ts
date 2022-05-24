import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { DxButtonModule, DxDataGridModule, DxFormModule, DxPopupModule, DxResponsiveBoxModule, DxSelectBoxModule, DxTextAreaModule, DxTextBoxModule, DxToastModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import { CreateWorkspaceComponent } from './pages/create-workspace/create-workspace.component';
import { DxiItemModule } from 'devextreme-angular/ui/nested';
import { ViewWorkspaceComponent } from './pages/view-workspace/view-workspace.component';
import { ListWorkspacesComponent } from './pages/list-workspaces/list-workspaces.component';
import { ViewBoardComponent } from './pages/view-board/view-board.component';

const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'workspaces/new',
    component: CreateWorkspaceComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'workspaces/:id/boards',
    component: ViewWorkspaceComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'workspaces/:workspaceId/boards/:boardId',
    component: ViewBoardComponent,
    canActivate: [ AuthGuardService ]
  },
  {
    path: 'workspaces/all',
    component: ListWorkspacesComponent,
    canActivate: [ AuthGuardService ],
    
  },
  {
    path: 'workspaces/private',
    component: ListWorkspacesComponent,
    canActivate: [ AuthGuardService ],
    
  },
  {
    path: 'workspaces/recent',
    component: ListWorkspacesComponent,
    canActivate: [ AuthGuardService ],
    
  },
  {
    path: 'workspaces/shared',
    component: ListWorkspacesComponent,
    canActivate: [ AuthGuardService ],
    
  },
  {
    path: 'workspaces/favorites',
    component: ListWorkspacesComponent,
    canActivate: [ AuthGuardService ],
    
  },
  {
    path: '**',
    redirectTo: '/home'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { 
      useHash: true,
      onSameUrlNavigation: 'reload'
    }), 
    DxDataGridModule, 
    DxFormModule,
    DxToastModule,
    CommonModule,
    DxiItemModule,
    DxTextAreaModule,
    DxPopupModule,
    DxSelectBoxModule,
    DxButtonModule,
    DxResponsiveBoxModule,
    DxTextBoxModule
  ],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [
    HomeComponent,
    ProfileComponent,
    CreateWorkspaceComponent,
    ViewWorkspaceComponent,
    ListWorkspacesComponent,
    ViewBoardComponent
  ]
})
export class AppRoutingModule { }
