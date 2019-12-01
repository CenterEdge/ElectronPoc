import { createAction, props } from '@ngrx/store';
import { createForParentAction } from './forParent';

export enum CommActionNames {
  Ping = '[Comm] Ping',
  Pong = '[Comm] Pong'
}

export const Ping = createAction(
  CommActionNames.Ping,
  props<{ id: number }>()
);

export const Pong = createForParentAction(
  CommActionNames.Pong,
  props<{ id: number }>()
);
