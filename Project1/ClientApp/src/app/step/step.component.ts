import { Component, OnInit } from '@angular/core';
import { ItemDTO, StepDTO, SwaggerClient } from '../services/SwaggerClient.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';


@Component({
  selector: 'app-step',
  templateUrl: './step.component.html',
  styleUrls: ['./step.component.scss']
})

export class StepComponent implements OnInit {
  steps: StepDTO[] = [];
  items: ItemDTO[] = [];
  currentStep: StepDTO | undefined;
  itemForm : FormGroup | undefined;
  submitted : boolean = false;
  constructor( private swagger: SwaggerClient ,private fb: FormBuilder) { }

  ngOnInit() {
     this.itemForm = this.fb.group({
      id: [''],
      title: ['', Validators.required],
      description: ['', Validators.required],
     });
     this.getAllSteps()
  }
  getAllSteps() {
    this.swagger.apiStepGetAllDetailsGet().subscribe(result => {
      this.steps = result;
      if(result.length > 0)
          this.currentStep = result[0];
    });
  }

  deleteStep(id: any)
  {
       this.submitted = false;
      this.swagger.apiStepDeleteDelete([id]).subscribe(result => {
        if(result.length > 0){
        let index = this.steps.findIndex(e => e.id === id); //find index in your array
        if (index > -1) {
          this.getAllSteps();
          this.clear();
        }    
      }
    });
  }
  addStep()
  {
    this.submitted = false;
      var stepNumber = 1;
      if(this.steps.length > 0)
      { 
        stepNumber = +(this.steps[this.steps.length - 1].name!.split(' ')[1]) + 1;
      }
      this.swagger.apiStepInsertPost([new StepDTO({ id: 0 , name : "Step " + stepNumber.toString()})]).subscribe(result => {
        if(result)
        {
          this.steps.push(result[0]);
          this.getStepItems(result[0].id);
        }
      });
  }
  getStepItems(id:any)
  {
    this.swagger.apiItemGetStepItemsGet(id).subscribe(res => {
      this.items = res;
      this.currentStep = this.steps.filter(x => x.id == id)[0];
    });
  }
  deleteItem(id: any)
  {
      this.swagger.apiItemDeleteDelete([id]).subscribe(result => {
        if(result.length > 0){
        let index = this.items.findIndex(e => e.id === id); //find index in your array
        if (index > -1) {
          this.items.splice(index, 1);
          this.clear();

        }    
      }
    });
  }

  onSubmit(form :FormGroup){
    this.submitted = true;
    if(!form.invalid)
    {
        let submitValue = Object.assign({}, form.value);
        submitValue.stepId = this.currentStep!.id;
        if(submitValue.id)
        {
          this.swagger.apiItemUpdatePut([submitValue]).subscribe(res => {
            this.getStepItems(this.currentStep!.id);
            this.clear();
          })
        }
        else{
            submitValue.id = 0;
            this.swagger.apiItemInsertPost([submitValue]).subscribe(res => {
            this.items.push(res[0]);})
            this.clear();

        }

    }
  }
  setFormWithItem(id : any) {
    let item = this.items.filter(e => e.id == id)[0];
    this.itemForm!.patchValue({
      id: item.id,
      title: item.title,
      description: item.description
    });
  }

  moveNextOrPrev(dir : any)
  {
    this.submitted = false;
    let index = this.steps.findIndex(e => e.id === this.currentStep?.id); //find index in your array
    if(dir === 'next')
    {
       if(this.steps[index+1])
       {
         this.getStepItems(this.steps[index+1].id)
       }
    }
    else
    {
        if(this.steps[index-1]){
          this.getStepItems(this.steps[index-1].id)
        }
    }
  }
  clear()
  {
    this.submitted = false;
    this.itemForm!.patchValue({
      id : '',
      title: '',
      description: '',
    });  
  }
}
