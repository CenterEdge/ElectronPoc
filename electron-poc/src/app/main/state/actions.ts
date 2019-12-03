import { createAction, props } from '@ngrx/store';
import { createForParentAction } from '../../actions/forParent';

export const TextChanged = createAction(
  '[Main] Text Changed',
  props<{ text: string }>()
);

export const MainResult = createForParentAction(
  '[Main] Main Result',
  props<{ text?: string, result: string }>()
);
