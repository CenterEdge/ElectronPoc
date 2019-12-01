import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ApplicationState } from '../reducers';
import { ModuleState } from '../reducers/main';

export const selectMainFeature = createFeatureSelector<
  ApplicationState,
  ModuleState
>('main');

export const selectText = createSelector(
  selectMainFeature,
  feature => feature ? feature.text : ''
);
