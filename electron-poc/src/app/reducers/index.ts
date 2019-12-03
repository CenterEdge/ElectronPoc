import {
  ActionReducer,
  ActionReducerMap,
  MetaReducer
} from '@ngrx/store';
import { RouterReducerState, BaseRouterStoreState } from '@ngrx/router-store';
import * as fromMain from '../main/state/reducers';
import * as fromRouter from './router';
import { environment } from '../../environments/environment';

export interface ApplicationState {
  main: fromMain.ModuleState;
  router: RouterReducerState<BaseRouterStoreState>;
}

export const initialState: ApplicationState = {
  main: fromMain.initialState,
  router: fromRouter.initialState
};

export function defaultReducer<T>(state: T) { return state; }

export const reducers: ActionReducerMap<ApplicationState> = {
  main: defaultReducer,
  router: fromRouter.reducer
};

export const metaReducers: MetaReducer<ApplicationState>[] = !environment.production ? [] : [];
