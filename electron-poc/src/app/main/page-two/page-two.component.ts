import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ApplicationState } from '../../reducers';
import { MainResult } from '../state/actions';
import { selectText } from '../state/selectors';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-page-two',
  templateUrl: './page-two.component.html',
  styleUrls: ['./page-two.component.scss']
})
export class PageTwoComponent implements OnInit {

  constructor(private store: Store<ApplicationState>) { }

  ngOnInit() {
  }

  dialogResult() {
    this.store.pipe(
      select(selectText),
      take(1)
    ).subscribe(text => {
      this.store.dispatch(MainResult({ text, result: 'ok' }));
    });
  }

  cancel() {
    this.store.dispatch(MainResult({ result: 'cancel' }));
  }
}
