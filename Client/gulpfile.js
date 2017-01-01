// gulp
var gulp = require('gulp');

// plugins
var jshint = require('gulp-jshint');
var uglify = require('gulp-uglify');
var minifyCSS = require('gulp-minify-css');
var clean = require('gulp-clean');
var runSequence = require('run-sequence');
var browserSync = require('browser-sync');
var reload = browserSync.reload;

// tasks
gulp.task('lint', function() {
    gulp.src(['./app/**/*.js', '!./app/bower_components/**'])
        .pipe(jshint())
        .pipe(jshint.reporter('default'))
        .pipe(jshint.reporter('fail'));
});
gulp.task('clean', function() {
    gulp.src('./dist/*')
        .pipe(clean({force: true}));
});
gulp.task('minify-css', function() {
    var opts = {comments:true,spare:true};
    gulp.src(['./app/css/*.css', '!./app/bower_components/**'])
        .pipe(minifyCSS(opts))
        .pipe(gulp.dest('./dist/css/'))
});
gulp.task('minify-js', function() {
    gulp.src(['./app/**/*.js', '!./app/bower_components/**'])
        .pipe(uglify({
            // inSourceMap:
            // outSourceMap: "app.js.map"
        }))
        .pipe(gulp.dest('./dist/'))
});
gulp.task('copy-bower-components', function () {
    gulp.src('./app/bower_components/**')
        .pipe(gulp.dest('dist/bower_components'));
});
gulp.task('copy-image', function () {
    gulp.src('./app/images/**')
        .pipe(gulp.dest('dist/images'));
});
gulp.task('copy-js', function () {
    gulp.src('./app/js/**')
        .pipe(gulp.dest('dist/js'));
});
gulp.task('copy-html-files', function () {
    gulp.src('./app/**/*.html')
        .pipe(gulp.dest('dist/'));
});

gulp.task('serve', [], function () {
    browserSync({
        notify: false,
        server: {
            baseDir: 'app/'
        }
    });

    gulp.watch(['app/**/*.html'], reload);
    gulp.watch(['app/**/*.js'], reload);
    gulp.watch(['app/css/*.css'], reload);
});

// default task
gulp.task('default',
    ['lint', 'serve']
);
gulp.task('build', function() {
    runSequence(
        ['clean'],
        ['lint', 'minify-css', 'copy-js', 'copy-image','copy-html-files', 'copy-bower-components']
    );
});