import { createAction, props } from '@ngrx/store';
import { createForParentAction } from './forParent';

export const NavigateTo = createAction(
  '[Main] Navigate To',
  props<{ route: string[]}>()
);

export const TextChanged = createAction(
  '[Main] Text Changed',
  props<{ text: string }>()
);

export const DialogResult = createForParentAction(
  '[Main] Dialog Result',
  props<{ result: string }>()
);
