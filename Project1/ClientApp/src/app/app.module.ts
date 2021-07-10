import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StepComponent } from './step/step.component';
import { ItemComponent } from './item/item.component';
import { HttpClientModule } from '@angular/common/http';
import { SwaggerClient } from './services/SwaggerClient.service';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    StepComponent,
    ItemComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [SwaggerClient],
  bootstrap: [AppComponent]
})
export class AppModule { }
