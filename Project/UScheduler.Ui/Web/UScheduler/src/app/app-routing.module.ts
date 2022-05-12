import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { DxButtonModule, DxDataGridModule, DxFormModule, DxPopupModule, DxSelectBoxModule, DxTextAreaModule, DxToastModule } from 'devextreme-angular';
import { CommonModule } from '@angular/common';
import { CreateWorkspaceComponent } from './pages/create-workspace/create-workspace.component';
import { DxiItemModule } from 'devextreme-angular/ui/nested';

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
    path: '**',
    redirectTo: '/home'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { useHash: true }), 
    DxDataGridModule, 
    DxFormModule,
    DxToastModule,
    CommonModule,
    DxiItemModule,
    DxTextAreaModule,
    DxPopupModule,
    DxSelectBoxModule,
    DxButtonModule,
  ],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [
    HomeComponent,
    ProfileComponent,
    CreateWorkspaceComponent
  ]
})
export class AppRoutingModule { }
