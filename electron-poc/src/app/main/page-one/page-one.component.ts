import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ApplicationState } from '../../reducers';
import { selectText } from '../state/selectors';
import { NavigateTo } from '../../actions/system';
import { TopRoutes } from '../../routes';
import { MainRoutes } from '../routes';
import { TextChanged } from '../state/actions';

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
    this.store.dispatch(NavigateTo({ route: [ TopRoutes.main, MainRoutes.pageTwo ] }));
  }

  change(value: string) {
    this.store.dispatch(TextChanged({ text: value }));
  }
}
