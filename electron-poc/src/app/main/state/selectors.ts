import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ApplicationState } from '../../reducers';
import { ModuleState, featureKey } from './reducers';

export const selectMainFeature = createFeatureSelector<
  ApplicationState,
  ModuleState
>(featureKey);

export const selectText = createSelector(
  selectMainFeature,
  feature => feature ? feature.text : ''
);
