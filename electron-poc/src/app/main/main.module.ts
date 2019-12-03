import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MainRoutingModule } from './main-routing.module';
import { MainComponent } from './main.component';
import { PageOneComponent } from './page-one/page-one.component';
import { PageTwoComponent } from './page-two/page-two.component';
import { StoreModule } from '@ngrx/store';
import * as fromFeature from './state/reducers';

@NgModule({
  declarations: [
    MainComponent,
    PageOneComponent,
    PageTwoComponent
  ],
  imports: [
    CommonModule,
    MainRoutingModule,
    StoreModule.forFeature(fromFeature.featureKey, fromFeature.reducer)
  ]
})
export class MainModule { }
