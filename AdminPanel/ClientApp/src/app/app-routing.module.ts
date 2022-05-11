import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidatesListComponent } from './components/candidates-list/candidates-list.component';
import { HomeComponent } from './components/home-component/home.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RegisterFormComponent } from './components/register-form/register-form.component';
import { SubjectsListComponent } from './components/subjects-list/subjects-list.component';
import { TestsListComponent } from './components/tests-list/tests-list.component';

const routes: Routes = [
  { path: 'login', component: LoginFormComponent },
  { path: 'register', component: RegisterFormComponent },
  { path: 'candidates', component: CandidatesListComponent },
  { path: 'subjects', component: SubjectsListComponent },
  { path: 'tests', component: TestsListComponent },
  { path: '', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
