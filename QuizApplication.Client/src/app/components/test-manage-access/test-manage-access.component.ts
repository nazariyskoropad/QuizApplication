import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TestAccessConfig } from 'src/app/models/test-access-config';
import { TestService } from 'src/app/services/test.service';

@Component({
  selector: 'app-test-manage-access',
  templateUrl: './test-manage-access.component.html',
  styleUrls: ['./test-manage-access.component.css']
})
export class TestManageAccessComponent implements OnInit {

  constructor(private testService: TestService,
              private route: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  testId: number;
  loading = true;
  testAccessConfigs;
  newTestAccessConfigs: TestAccessConfig[];
  addLinksForm: FormGroup;
  links: FormArray;


  ngOnInit() {
    this.loadTestConfig();
    this.createForm();
    this.newTestAccessConfigs = new Array();
  }

  private loadTestConfig() {
    this.testId = Number(this.route.snapshot.paramMap.get('id'));
    this.testService.getTestAccessConfig(this.testId).subscribe((data: TestAccessConfig) => {
      this.testAccessConfigs = data;
      console.log(this.testAccessConfigs);
      this.loading = false;
    },
    error => {
      console.log(error);
    })
  }

  get f() {
    return this.addLinksForm.controls;
  }

  createForm() {
    this.addLinksForm = this.fb.group({
      links: this.fb.array([])
    });
  }

  createFormElement() {
    return this.fb.group({
      UserName: ['', [ Validators.maxLength(200)]]
    })
  }

  addLink(){
    this.links = this.addLinksForm.get('links') as FormArray;
    this.links.push(this.createFormElement());
  }

  deleteLink(testAccessConfigId: number) {
    this.testService.deleteLink(testAccessConfigId).subscribe(() => {
      this.ngOnInit();
    },
    error => {
      console.log(error);
    })
  }

  submitForm() {
    for (let item of this.f.links.value) {
      let testAccessConfig = new TestAccessConfig();
      testAccessConfig.testId = this.testId;
      testAccessConfig.userName = item.UserName;

      this.newTestAccessConfigs.push(testAccessConfig);
    }

    this.testService.createLinks(this.testId, this.newTestAccessConfigs).subscribe(() => {
      this.ngOnInit();
    },
    error => {
      console.log(error);
    })
  }
}
