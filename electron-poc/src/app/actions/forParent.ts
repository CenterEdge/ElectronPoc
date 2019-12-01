import { TypedAction, ActionCreator, Creator } from '@ngrx/store/src/models';

export interface ForParentAction<T extends string> extends TypedAction<T> {
  forParent: true;
}

// tslint:disable-next-line:max-line-length
export const createForParentAction = <T extends string, P extends object>(type: T, config: {
  _as: 'props';
  _p: P;
}): ActionCreator<T, (props: P) => P & ForParentAction<T>> => {
  return defineType(type, (props2: P) => ({
    ...props2,
    type,
    forParent: true
  })) as ActionCreator<T, (props: P) => P & ForParentAction<T>>;
};

function defineType(type: string, creator: Creator): Creator {
  return Object.defineProperty(
    Object.defineProperty(creator, 'type', {
      value: type,
      writable: false,
    }),
    'forParent', {
      value: true,
      writable: false
    });
}
