import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DxButtonModule, DxFormModule, DxPopupModule } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { Board, BoardsService } from '../../services/boards.service';

@Component({
  selector: 'app-board-card',
  templateUrl: './board-card.component.html',
  styleUrls: ['./board-card.component.scss']
})
export class BoardCardComponent implements OnInit {
  board: Board | null;
  editBoardPopupIsVisible: boolean;

  constructor(private boardService: BoardsService, private router: Router) { 
    this.board = new Board();
    this.editBoardPopupIsVisible = false;
  }

  deleteBoard() {
    console.log(`Deleting board '${this.board!.title}'`);
    try {
      this.boardService.delete(this.board!);
      this.board = null;
    }
    catch (e){
      console.log(e);
      notify('Error when deleting board. Check the console for details', 'error');
    }
  }

  openBoard() {
    this.router.navigateByUrl(`/workspaces/${this.board?.workspaceId}/boards/${this.board?.id}`);
  }

  togleEditBoardPopup() {
    this.editBoardPopupIsVisible = true;
  }

  async editBoard(e: any) {
    e.preventDefault();
    console.log(`Updating board '${this.board!.title}'`);
    console.log(this.board);
    try {
      this.board = await this.boardService.update(this.board!, this.board?.id!);
      notify(`Board '${this.board.title}' is updated.`, 'success');
      this.editBoardPopupIsVisible = false;
    }
    catch (e){
      console.log(e);
      notify('Error when deleting board. Check the console for details', 'error');
    }
  }

  ngOnInit(): void {
  }
}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule,
    DxPopupModule,
    DxFormModule
  ],
  declarations: [ BoardCardComponent ],
  exports: [ BoardCardComponent ]
})
export class BoardCardModule { }
