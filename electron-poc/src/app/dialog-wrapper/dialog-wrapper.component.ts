import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-dialog-wrapper',
  templateUrl: './dialog-wrapper.component.html',
  styleUrls: ['./dialog-wrapper.component.scss']
})
export class DialogWrapperComponent implements OnInit {

  @Input() title = 'Title';

  constructor() { }

  ngOnInit() {
  }

}
