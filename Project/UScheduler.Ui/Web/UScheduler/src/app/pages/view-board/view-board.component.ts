import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import notify from 'devextreme/ui/notify';
import { TaskCardComponent } from 'src/app/shared/components/task-card/task-card.component';
import { Board, BoardsService } from 'src/app/shared/services/boards.service';
import { Task, TasksService } from 'src/app/shared/services/tasks.service';

@Component({
  selector: 'app-view-board',
  templateUrl: './view-board.component.html',
  styleUrls: ['./view-board.component.scss']
})
export class ViewBoardComponent implements OnInit {
  @ViewChild('tasksCards', { read: ViewContainerRef }) tasksCards!: ViewContainerRef;
  workspaceId: string;
  boardId: string;
  board: Board | null;
  editBoardPopupIsVisible: boolean;
  createTaskPopupIsVisible: boolean;
  task: Task;
  tasks!: Array<Task>;

  constructor(private boardService: BoardsService, 
    private tasksService: TasksService, 
    private route: ActivatedRoute, 
    private router: Router) {

    this.workspaceId = this.route.snapshot.params['workspaceId'];
    this.boardId = this.route.snapshot.params['boardId'];
    this.board = new Board();
    this.task = new Task();
    this.task.boardId = this.boardId;
    this.editBoardPopupIsVisible = false;
    this.createTaskPopupIsVisible = false;
  }

  async ngOnInit(): Promise<void> {
    this.board = await this.boardService.getById(this.workspaceId, this.boardId);
    await this.initTasks();
  }

  async initTasks(){
    this.tasks = await this.tasksService.getTasksFromBord(this.workspaceId, this.boardId);
    console.log(this.tasks.length);
    if (this.tasks.length == 0) {
      // TODO
    }
    else {
      this.tasks.forEach(task => {
        var component = this.tasksCards.createComponent(TaskCardComponent);
        component.instance.task = task;
        component.instance.workspaceId = this.workspaceId;
      });
    }
  }

  deleteBoard() {
    console.log(`Deleting board '${this.board!.title}'`);
    try {
      this.boardService.delete(this.board!);
      this.router.navigateByUrl(`/workspaces/${this.workspaceId}/boards`)
      this.board = null;
    }
    catch (e){
      console.log(e);
      notify('Error when deleting board. Check the console for details', 'error');
    }
  }

  togleEditBoardPopup() {
    this.editBoardPopupIsVisible = true;
  }

  togleCreateTaskPopup() {
    this.createTaskPopupIsVisible = true;
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

  async createTask(e: any) {
    e.preventDefault();
    console.log(`Creating task '${this.task.title}'`);
    console.log(this.task);
    try {
      await this.tasksService.create(this.task, this.workspaceId);
      notify(`Task '${this.task.title}' is created.`, 'success');
      this.createTaskPopupIsVisible = false;
      this.task = new Task();
      this.task.boardId = this.boardId;
    }
    catch (e){
      console.log(e);
      notify('Error when crating task. Check the console for details', 'error');
    }
  }
}
