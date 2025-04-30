# Quiz
Made for this university assignement:

As part of the Quiz task, you should prepare an application consisting of two modules: quiz generator, quiz solving. Requirements:
1. The task should be done in pairs. One person creates the quiz creation application, the other the quiz solving application.
2. You should discuss and design the appropriate classes that make up the object model of the described problem, such as Quiz, Question, Answer, etc. classes.
3. The quiz generator should have the following functionalities:
- creating a new quiz with a given name,
- adding, removing and modifying questions belonging to a given quiz,
- saving the quiz to a disk file in an encrypted form (e.g. with the AES algorithm),
- loading an existing quiz from a disk file (decryption) and the possibility of further editing it,
- user-friendly interface that protects against making mistakes.
4. The question has four answers, of which at least one is correct (multiple choice question).  The quiz-taking application should have the following functionality:
- loading the quiz from a disk file (decryption),
- buttons Start, End to start and end the quiz. Depending on the application status, the buttons should be enabled or disabled,
- a timer to measure the time of solving the quiz,
- a user-friendly interface to prevent mistakes,
- a system for calculating points after the quiz is completed,
- the ability to check correct answers after solving the quiz.

For a correctly executed project in accordance with the MVVM pattern, it is possible to obtain 20
points. Without the MVVM pattern, at most 7 points.

# Organization
1. Mateusz is in charge of the quiz maker, Kacper is in charge of the quiz solver.
2. Veriable and files should have understandable names in English
3. For each feature of the application create a separete branch coming out of main. And when the feature is ready, create a merge request and ask the other person for a review. After possible review changes and approval, the branch gets merged to main.
