import { createReducer, on, Action } from '@ngrx/store';
import * as MainActions from '../actions/main';

export interface ModuleState {
  text: string;
}

export const initialState: ModuleState = {
  text: ''
};

const mainReducer = createReducer(
  initialState,
  on(MainActions.TextChanged, (state, action) => ({ ...state, text: action.text })),
);

export function reducer(state: ModuleState | undefined, action: Action) {
  return mainReducer(state, action);
}
