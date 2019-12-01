import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { tap, filter, mergeMap } from 'rxjs/operators';
import * as CommActions from '../actions/comm';
import { CommService } from '../services/comm';
import { ForParentAction } from '../actions/forParent';

@Injectable()
export class CommEffects {
  transmitActions$ = createEffect(() => this.actions$.pipe(
    filter(action => (action as ForParentAction<any>).forParent === true),
    tap(action => this.commService.sendActions([action]))
  ),
  {
    dispatch: false
  });

  ping$ = createEffect(() => this.actions$.pipe(
    ofType(CommActions.Ping),
    mergeMap(action => [CommActions.Pong({ id: action.id })])
  ));

  constructor(
    private actions$: Actions,
    private commService: CommService) {}
}
