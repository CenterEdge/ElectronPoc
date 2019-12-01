import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { CommEffects } from './comm.effects';

describe('CommEffects', () => {
  let actions$: Observable<any>;
  let effects: CommEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        CommEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get<CommEffects>(CommEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
