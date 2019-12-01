import { Injectable, NgZone } from '@angular/core';
import { Store, Action } from '@ngrx/store';
import { ApplicationState } from '../reducers';
import { ipcRenderer } from 'electron';

@Injectable({
  providedIn: 'root'
})
export class CommService {
  constructor(private store: Store<ApplicationState>, private ngZone: NgZone) {
    ipcRenderer.on('actions', (_, actions) => {
      if (actions) {
        ngZone.run(() => {
          actions.forEach(action => {
            this.store.dispatch(action);
          });
        });
      }
    });
  }

  sendActions(actions: Action[]) {
    ipcRenderer.send('actions', actions);
  }
}
