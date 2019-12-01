import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { tap } from 'rxjs/operators';
import * as MainActions from '../actions/main';
import { MainService } from '../services/main';

@Injectable()
export class MainEffects {
  sendPacket$ = createEffect(() => this.actions$.pipe(
    ofType(MainActions.NavigateTo),
    tap(action => this.mainService.navigateTo(action.route))
  ),
  {
    dispatch: false
  });

  constructor(
    private actions$: Actions,
    private mainService: MainService) {}
}
