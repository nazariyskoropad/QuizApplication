import { Component, OnInit } from '@angular/core';
import { TestService } from 'src/app/services/test.service';
import { TestDetailed } from 'src/app/models/test-detailed';
import { Router, ActivatedRoute } from '@angular/router';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { QApair, UserAnswers } from 'src/app/models/user-answers';
import { TestResult } from 'src/app/models/test-result';
import { TestAccessConfig } from 'src/app/models/test-access-config';

@Component({
  selector: 'app-test-pass',
  templateUrl: './test-pass.component.html',
  styleUrls: ['./test-pass.component.css']
})
export class TestPassComponent implements OnInit {

  constructor(private testService: TestService,
              private router: Router,
              private route: ActivatedRoute,
              private fb: FormBuilder) { }

  loading = false;
  passed = false;
  submitted = false;
  startTestConfirmed = false;
  nameEntered = false;
  selectedIndex: number = 0;
  testId: number;
  timeToPassTest: number;
  hours: number
  minutes: number;
  seconds: number;
  uniqueLink: string
  startedAt: Date;
  test: TestDetailed;
  testAccessConfig: TestAccessConfig;
  userAnswers: UserAnswers;
  testResult: TestResult;
  passForm: FormGroup;

  ngOnInit() {
    this.loadTestInfo();
  }

  startTimer() {
    this.hours = Math.floor(this.timeToPassTest / 3600)
    this.minutes = Math.floor(this.timeToPassTest % 3600 / 60);
    this.seconds = this.timeToPassTest % 60;

    setInterval(() => {     
      this.timeToPassTest--;

      this.hours = Math.floor(this.timeToPassTest / 3600)
      this.minutes = Math.floor(this.timeToPassTest % 3600 / 60);
      this.seconds = this.timeToPassTest % 60;

      if(this.minutes === 0 && this.seconds === 0)
      {
        this.finishTest();
      }

    }, 1000)
  }

  previousQuestion() {
    if (this.selectedIndex != 0) {
      this.selectedIndex -= 1;
    }
  }

  nextQuestion() {
    if (this.selectedIndex != this.test.questions.length) {
      this.selectedIndex += 1;
    }
  }

  startTest() {
    this.nameEntered = true;

    if (this.passForm.invalid) {
      return;
    }

    this.startTestConfirmed = true;

    this.userAnswers = new UserAnswers();
    this.userAnswers.userName = this.f.TestUserName.value;
    this.nameEntered = true;

    this.startedAt = new Date();
    this.startTimer();
  }

  loadTestInfo() {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.uniqueLink = this.route.snapshot.paramMap.get('link');


    this.testService.getTestToPass(this.testId, this.uniqueLink).subscribe((data: TestAccessConfig) => {
      this.testAccessConfig = data;
      this.test = data.test;
      this.initForm();
      this.loading = true;
      this.timeToPassTest = data.test.timeLimit * 60; //in seconds;
    },
    error => {
      console.log(error);
    });
  }

  private initForm() {
    this.passForm = this.fb.group({
      TestUserName: [this.testAccessConfig.userName, [Validators.required, Validators.maxLength(200)]],
      Questions: this.fb.array([])
    });


    for(let question of this.test.questions)
    {
      let q = this.fb.group({
        Id: [question.id],
        QuestionDescription: [question.description],
        QuestionAnswers: this.fb.array([])
      });
      
      for(let questionAnswer of question.questionAnswers)
      {
        const qa = this.fb.group({
          Id: [questionAnswer.id],
          QaDescription: [questionAnswer.description],
          QaIsTrue: [false]
        });
        (q.controls.QuestionAnswers as FormArray).push(qa);
      }
      this.questionForms.push(q);
    }   
  }

  get questionForms() {
    return this.passForm.get('Questions') as FormArray;
  }

  get f() {
    return this.passForm.controls;
  }

  finishTest() {
    this.submitted = true;

    if (this.passForm.invalid) {
      console.log("im here");
      return;
    }

    this.initUserAnswers();
  }

  initUserAnswers() {
    if(!this.userAnswers) {
      this.userAnswers = new UserAnswers();
    }

    this.userAnswers.endedAt = new Date();
    this.userAnswers.startedAt = this.startedAt;

    if(this.testAccessConfig.userName) {
      this.userAnswers.userName = this.testAccessConfig.userName;
    }

    for (let question of this.f.Questions.value) {
      for(let qa of question.QuestionAnswers) {
        if(qa.QaIsTrue)
        {
          let qaPair = new QApair();
          qaPair.answerId = qa.Id;
          qaPair.questionId = question.Id;
          this.userAnswers.QApairs.push(qaPair);
        }
      }
    }

    this.testService.passUserAnswers(this.userAnswers, this.testId)
    .subscribe((data: any) => {
      this.testResult = data;
      this.passed = true;
    })
  }
}
