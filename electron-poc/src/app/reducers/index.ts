import {
  ActionReducer,
  ActionReducerMap,
  MetaReducer
} from '@ngrx/store';
import { RouterReducerState, BaseRouterStoreState } from '@ngrx/router-store';
import * as fromMain from './main';
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

export const reducers: ActionReducerMap<ApplicationState> = {
  main: fromMain.reducer,
  router: fromRouter.reducer
};

export const metaReducers: MetaReducer<ApplicationState>[] = !environment.production ? [] : [];