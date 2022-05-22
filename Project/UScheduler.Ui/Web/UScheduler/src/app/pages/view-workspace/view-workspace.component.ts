import { HttpErrorResponse } from '@angular/common/http';
import { Component, ComponentFactoryResolver, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import notify from 'devextreme/ui/notify';
import { lastValueFrom } from 'rxjs';
import { BoardCardComponent } from 'src/app/shared/components/board-card/board-card.component';
import { Board, BoardsService } from 'src/app/shared/services/boards.service';
import { Workspace, WorkspacesService } from 'src/app/shared/services/workspaces.service';

@Component({
  selector: 'app-view-workspace',
  templateUrl: './view-workspace.component.html',
  styleUrls: ['./view-workspace.component.scss']
})
export class ViewWorkspaceComponent implements OnInit {
  workspaceId!: string;
  workspace: Workspace;
  boards!: Array<Board>;
  createBoardPopupIsVisible: boolean;
  createBoardPopupTitle!: string;
  board: Board; 
  @ViewChild('boardsCards', { read: ViewContainerRef }) boardsCards!: ViewContainerRef;


  constructor(
    private route: ActivatedRoute, 
    private router: Router, 
    private workspaceService: WorkspacesService, 
    private boardsService: BoardsService) { 
    
    this.workspace = new Workspace();
    this.createBoardPopupIsVisible = false
    this.board = new Board();
  }

  async ngOnInit(): Promise<void> {
    this.workspaceId = this.route.snapshot.params['id'];
    
    try {
      this.workspace = await this.workspaceService.getById(this.workspaceId);
      this.createBoardPopupTitle = `Create board in workspace '${this.workspace.title}'`;
      this.initBoards();
    }
    catch (e) {
      if ( e instanceof HttpErrorResponse) {
        if (e.status == 404) {
          this.router.navigateByUrl("/");
        }
      }
    }
  }

  async initBoards(){
    this.boards = await this.boardsService.getBoardsFromWorksopace(this.workspaceId);
    console.log(this.boards.length);
    if (this.boards.length == 0) {
      // TODO
    }
    else {
      this.boards.forEach(board => {
        var component = this.boardsCards.createComponent(BoardCardComponent);
        component.instance.board = board;
      });
    }
  }

  toggleCreateBoardPopup() {
    this.createBoardPopupIsVisible = true;
  }

  async createBoard(e: any) {
    e.preventDefault();
    try {
      let createdBoard = await this.boardsService.create(this.board, this.workspaceId);
      this.boards = this.boards.concat(createdBoard);
      var component = this.boardsCards.createComponent(BoardCardComponent);
      component.instance.board = createdBoard;
      this.createBoardPopupIsVisible = false;
      this.board = new Board();
      notify(`Board '${createdBoard.title}' was successfully created within '${this.workspace.title}' workspace.`, 'success');
    }
    catch (e) {
      console.log(e);
      notify('Error when creating board. Check the console for details', 'error');
    }
  }

  async deleteWorkspace() {
    try {
      await this.workspaceService.delete(this.workspaceId);
      notify('Workspace deleted successuflly', 'success');
      this.router.navigateByUrl("/");
    }
    catch (e){
      console.log(e);
      notify('Error when deleting workspaces. Check the console for details', 'error');
    }
  }
}
