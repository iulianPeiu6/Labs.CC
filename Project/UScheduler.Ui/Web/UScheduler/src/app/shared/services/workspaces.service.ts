import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { lastValueFrom } from 'rxjs';
import { environment as env } from 'src/environments/environment';
import { Board } from './boards.service';

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
  constructor(private http: HttpClient, private auth: AuthService) {
    
  }

  async create(workspace: Workspace):Promise<Workspace> {
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

    let response$ = this.http.post<Workspace>(`${env.apiEndpoint}/api/v1/Workspaces`, workspaceToCreate, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async update(workspace: Workspace):Promise<Workspace> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);
    
    const workspaceToUpdate = {
      title: workspace.title,
      description: workspace.description,
      accessLevel: workspace.accessLevel
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.put<Workspace>(`${env.apiEndpoint}/api/v1/Workspaces/${workspace.id}`, workspaceToUpdate, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async getById(id: String): Promise<Workspace> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.get<Workspace>(`${env.apiEndpoint}/api/v1/Workspaces/${id}`, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }

  async filter(by: string): Promise<Array<Workspace>> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);
    let user = await lastValueFrom(this.auth.getUser());

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.get<Array<Workspace>>(`${env.apiEndpoint}/api/v1/Workspaces?owner=${user?.email}`, { headers: headers});
    let response = await lastValueFrom(response$);

    if (by.toLowerCase() == "private") {
      response = response.filter(workspace => workspace.accessLevel == "Private");
    } 
    else if (by.toLowerCase() == "shared") {
      response = response.filter(workspace => workspace.colabs.length > 1);
    }
    else if (by.toLowerCase() == "recent") {
      response = response.sort((w1, w2) => w1.updatedAt > w2.updatedAt ? -1 : 1);
    }
    console.log(response);

    return response;
  }

  async delete(id: String): Promise<Workspace> {
    let token$ = this.auth.getAccessTokenSilently();
    let token = await lastValueFrom(token$);

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    })

    let response$ = this.http.delete<Workspace>(`${env.apiEndpoint}/api/v1/Workspaces/${id}`, { headers: headers});
    let response = await lastValueFrom(response$);
    console.log(response);

    return response;
  }
}