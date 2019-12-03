import { createAction, props } from '@ngrx/store';

export const NavigateTo = createAction(
  '[System] Navigate To',
  props<{ route: string[]}>()
);
