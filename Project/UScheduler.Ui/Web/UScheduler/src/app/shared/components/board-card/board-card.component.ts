import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { DxButtonModule } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { Board, BoardsService } from '../../services/boards.service';

@Component({
  selector: 'app-board-card',
  templateUrl: './board-card.component.html',
  styleUrls: ['./board-card.component.scss']
})
export class BoardCardComponent implements OnInit {
  board: Board | null;

  constructor(private boardService: BoardsService) { 
    this.board = new Board();
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

  ngOnInit(): void {
  }
}

@NgModule({
  imports: [
    DxButtonModule,
    CommonModule
  ],
  declarations: [ BoardCardComponent ],
  exports: [ BoardCardComponent ]
})
export class BoardCardModule { }
