import { createFeatureSelector, createSelector } from '@ngrx/store';
import { RouterReducerState, BaseRouterStoreState, getSelectors } from '@ngrx/router-store';
import { ApplicationState } from '../reducers';

export const selectRouter = createFeatureSelector<
  ApplicationState,
  RouterReducerState<BaseRouterStoreState>
>('router');

export const {
  selectQueryParams,    // select the current route query params
  selectQueryParam,     // factory function to select a query param
  selectRouteParams,    // select the current route params
  selectRouteParam,     // factory function to select a route param
  selectRouteData,      // select the current route data
  selectUrl,            // select the current url
} = getSelectors(selectRouter);
