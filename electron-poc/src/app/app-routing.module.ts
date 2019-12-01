import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PageOneComponent } from './page-one/page-one.component';
import { PageTwoComponent } from './page-two/page-two.component';
import { BlankPageComponent } from './blank-page/blank-page.component';
import { TopRoutes } from './routes';

const routes: Routes = [
  { path: TopRoutes.pageOne, component: PageOneComponent },
  { path: TopRoutes.pageTwo, component: PageTwoComponent },
  { path: '**', component: BlankPageComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
