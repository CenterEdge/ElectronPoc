import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ApplicationState } from '../reducers';
import { selectText } from '../selectors/main';
import { NavigateTo } from '../actions/main';
import { TopRoutes } from '../routes';

@Component({
  selector: 'app-page-one',
  templateUrl: './page-one.component.html',
  styleUrls: ['./page-one.component.scss']
})
export class PageOneComponent implements OnInit {

  text$ = this.store.pipe(
    select(selectText)
  );

  constructor(private store: Store<ApplicationState>) { }

  ngOnInit() {
  }

  next() {
    this.store.dispatch(NavigateTo({ route: [ TopRoutes.pageTwo ] }));
  }
}
