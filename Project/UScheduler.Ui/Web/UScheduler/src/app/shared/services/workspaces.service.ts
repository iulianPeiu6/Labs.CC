import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { lastValueFrom } from 'rxjs';

export class Workspace {
    id!: string;
    title!: string;
    description!: string;
    owner!: string;
    accessLevel!: string;
    colabs!: Array<string>;
    workspaceTemplate!: string;
    createdAt!: Date;
    createdBy!: string;
    updatedAt!: Date;
    updatedBy!: string;
}

@Injectable()
export class WorkspacesService {
  constructor(private http: HttpClient, private auth: AuthService) {}

  async createWorkspace(workspace: Workspace):Promise<Workspace> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);
    
    const workspaceToCreate = {
      title: workspace.title,
      description: workspace.description,
      accessLevel: workspace.accessLevel,
      workspaceTemplate: workspace.workspaceTemplate
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.post<Workspace>('/api/v1/Workspaces', workspaceToCreate, { headers: headers});
    let response = await lastValueFrom(response$);

    console.log(response);

    return response;
  }
}