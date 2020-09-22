import { Component, OnInit } from '@angular/core';
import { TestDetailed } from 'src/app/models/test-detailed';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TestService } from 'src/app/services/test.service';
import { QuestionAnswer } from 'src/app/models/questionAnswer';
import { Question } from 'src/app/models/question';

@Component({
  selector: 'app-test-edit',
  templateUrl: './test-edit.component.html',
  styleUrls: ['./test-edit.component.css']
})
export class TestEditComponent implements OnInit {

  loading = false;
  testId: number;
  test: TestDetailed;
  editForm: FormGroup;
  submitted = false;

  constructor(public testService: TestService,
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder) {
    }

  ngOnInit() {
    this.loadTestInfo();
  }

  loadTestInfo() {
    this.testId = Number(this.route.snapshot.paramMap.get('id'));
    this.testService.getTest(this.testId).subscribe((data: TestDetailed) => {
      this.test = data;
      this.loading = false;
      this.initForm();
    },
    error => {
      console.log(error);
    });
  }

  private initForm() {
    this.editForm = this.fb.group({
      TestName: [this.test.name, [Validators.required, Validators.maxLength(200)]],
      TestDescription: [this.test.description, [Validators.required, Validators.maxLength(200)]],
      TestTimeLimit: [this.test.timeLimit, Validators.required],
      TestPoints: [this.test.points, Validators.required],
      TestStartsAt: [this.test.startsAt, Validators.required],
      TestEndsAt: [this.test.endsAt, Validators.required],
      Questions: this.fb.array([])
    });


    for(let question of this.test.questions)
    {
      let q = this.fb.group({
        QuestionTimeLimit: [question.timeLimit, Validators.required],
        QuestionPoints: [question.points, Validators.required],
        QuestionDescription: [question.description, [Validators.required, Validators.maxLength(700)]],
        QuestionAnswers: this.fb.array([])
      });
      
      for(let questionAnswer of question.questionAnswers)
      {
        const qa = this.fb.group({
          QaDescription: [questionAnswer.description, [Validators.required, Validators.maxLength(700)]],
          QaIsTrue: [questionAnswer.isCorrect, Validators.required]
        });
        (q.controls.QuestionAnswers as FormArray).push(qa);
      }
      this.questionForms.push(q);
    }   
  }

  get questionForms() {
    return this.editForm.get('Questions') as FormArray;
  }

  addQuestion() {
    const question = this.fb.group({

      QuestionTimeLimit: ['', Validators.required],
      QuestionPoints: ['', Validators.required],
      QuestionDescription: ['', [Validators.required, Validators.maxLength(700)]],
      QuestionAnswers: this.fb.array([])
    });

    this.questionForms.push(question);
  }

  addQA(question) {
    const qa = this.fb.group({
      QaDescription: ['', [Validators.required, Validators.maxLength(700)]],
      QaIsTrue: [false, Validators.required]
    });

    question.get("QuestionAnswers").push(qa);
  }

  deleteQuestion(i) {
    this.questionForms.removeAt(i);
  }

  deleteQA(question, i) {
    question.get("QuestionAnswers").removeAt(i);
  }

  get f() {
    return this.editForm.controls;
  }

  edit(){
    this.submitted = true;

    if (this.editForm.invalid) {
      return;
    }

    if(!this.validatePoints())
    {
      alert('Sum of points for question is not equal to test points')
      return;
    }

    this.initializeTest();

    this.testService.editTest(this.test, this.testId).subscribe((data: TestDetailed) => {
      alert('Tests edited successfully!');
      this.router.navigate(['/admin/tests', data.id]);
    }, error => {
      console.log(error);
    })
  } 

  validatePoints() {
    let questionPoints = 0;
    this.f.Questions.value.forEach(q => questionPoints += q.QuestionPoints)
    return this.f.TestPoints.value == questionPoints;
  }

  initializeTest() {
    this.test = new TestDetailed();

    this.test.id = this.testId;
    this.test.name = this.f.TestName.value;
    this.test.description = this.f.TestDescription.value;
    this.test.points = this.f.TestPoints.value;
    this.test.timeLimit = this.f.TestTimeLimit.value;
    this.test.startsAt = this.f.TestStartsAt.value;
    this.test.endsAt = this.f.TestEndsAt.value;
    
    for (let question of this.f.Questions.value) {
      let newQuestion = new Question()

      newQuestion.testId = this.testId;
      newQuestion.description = question.QuestionDescription;
      newQuestion.points = question.QuestionPoints;
      newQuestion.timeLimit = question.QuestionTimeLimit;

      for(let qa of question.QuestionAnswers) {
        let newQa = new QuestionAnswer();
        
        newQa.isCorrect = qa.QaIsTrue;
        newQa.description = qa.QaDescription;

        newQuestion.questionAnswers.push(newQa);
      }

      this.test.questions.push(newQuestion);
    }
  }
}
