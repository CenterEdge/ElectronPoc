import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { tap } from 'rxjs/operators';
import * as SystemActions from '../actions/system';
import { SystemService } from '../services/system';

@Injectable()
export class SystemEffects {
  navigateTo$ = createEffect(() => this.actions$.pipe(
    ofType(SystemActions.NavigateTo),
    tap(action => this.systemService.navigateTo(action.route))
  ),
  {
    dispatch: false
  });

  constructor(
    private actions$: Actions,
    private systemService: SystemService) {}
}
