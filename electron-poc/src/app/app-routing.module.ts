import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlankPageComponent } from './blank-page/blank-page.component';
import { TopRoutes } from './routes';

const routes: Routes = [
  { path: TopRoutes.main, loadChildren: () => import('./main/main.module').then(m => m.MainModule) },
  { path: '**', component: BlankPageComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
