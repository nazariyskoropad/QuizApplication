import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestPassComponent } from './test-pass.component';

describe('TestPassComponent', () => {
  let component: TestPassComponent;
  let fixture: ComponentFixture<TestPassComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestPassComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestPassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
