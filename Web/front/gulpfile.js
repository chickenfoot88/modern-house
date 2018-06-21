var gulp           = require('gulp'),
		gutil          = require('gulp-util' ),
		sass           = require('gulp-sass'),
		concat         = require('gulp-concat'),
		uglify         = require('gulp-uglify'),
		cleanCSS       = require('gulp-clean-css'),
		rename         = require('gulp-rename'),
		del            = require('del'),
		imagemin       = require('gulp-imagemin'),
		cache          = require('gulp-cache'),
		autoprefixer   = require('gulp-autoprefixer'),
		bourbon        = require('node-bourbon'),
		ftp            = require('vinyl-ftp'),
		notify         = require('gulp-notify'),
		multiDest			 = require('gulp-multi-dest');

// Скрипты проекта
gulp.task('scripts', function() {
	return gulp.src([
		'app/js/maps.js',
		'app/js/dateRangeFilter.js',
		'app/js/common.js', // Всегда в конце
		'app/js/tables.js',
		'app/js/tables.requests.js'
		])
	.pipe(concat('scripts.js'))
	// .pipe(uglify())
	.pipe(multiDest(['app/js', '../tbo.webhost/scripts']), 0755)
});

gulp.task('scripts-min', function() {
	return gulp.src([
		'app/js/scripts.js'
	])
	.pipe(rename({suffix: '.min', prefix : ''}))
	.pipe(uglify())
	.pipe(multiDest(['app/js', '../tbo.webhost/scripts']), 0755)
});

// Скрипты библиотек
gulp.task('libs', function() {
	return gulp.src([
		'app/libs/jquery/dist/jquery.min.js',
		'app/libs/moment/min/moment.min.js',
		'app/libs/bootstrap/dist/js/bootstrap.min.js',
		'app/libs/admin-lte/dist/js/adminlte.min.js',
		'app/libs/datatables.net/js/jquery.dataTables.min.js',
		'app/libs/datatables.net-bs/js/dataTables.bootstrap.min.js',
		'app/libs/select2/dist/js/select2.full.min.js',
		'app/libs/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js',
		'app/libs/bootstrap-datepicker/dist/locales/bootstrap-datepicker.ru.min.js',
		'app/libs/bootstrap-daterangepicker/daterangepicker.js',
		'app/libs/bootstrap-datetime-picker/js/bootstrap-datetimepicker.min.js',
		'app/libs/bootstrap-datetime-picker/js/locales/bootstrap-datetimepicker.ru.js',
		'app/libs/fontawesome-markers/fontawesome-markers.min.js',
		'app/libs/icheck/icheck.min.js',
		'app/libs/datetime-moment/datetime-moment.js',
		'app/libs/inputmask/dist/jquery.inputmask.bundle.js',
		'app/libs/js-marker-clusterer/src/markerclusterer.js',
		])
	.pipe(concat('libs.min.js'))
	.pipe(uglify())
	.pipe(multiDest(['app/js', '../tbo.webhost/scripts']), 0755)
});

gulp.task('sass', function() {
	return gulp.src('app/sass/**/*.sass')
	.pipe(sass({
		includePaths: bourbon.includePaths
	}).on("error", notify.onError()))
	.pipe(rename({suffix: '.min', prefix : ''}))
	.pipe(autoprefixer(['last 15 versions']))
	.pipe(cleanCSS())
	.pipe(multiDest(['app/css', '../tbo.webhost/content']), 0755)
});

gulp.task('watch', ['sass', 'scripts', 'scripts-min'], function() {
	gulp.watch('app/sass/**/*.sass', ['sass']);
	gulp.watch(['libs/**/*.js', 'app/js/*.js',  '!app/js/scripts.min.js'], ['scripts']);
	gulp.watch(['app/*.html', 'app/content/**/*.html']);
});

gulp.task('imagemin', function() {
	return gulp.src('app/img/**/*')
	.pipe(cache(imagemin()))
	.pipe(multiDest(['dist/img', '../tbo.webhost/content/img']), 0755);
});

gulp.task('build', ['removedist', 'imagemin', 'sass', 'scripts', 'scripts-min'], function() {

	var buildFiles = gulp.src([
		'app/*.html',
		'app/.htaccess',
		]).pipe(gulp.dest('dist'));

	var buildManager = gulp.src([
		'app/content/manager/*.html'
	]).pipe(gulp.dest('dist/content/manager'));

	var buildDirector = gulp.src([
		'app/content/director/*.html'
	]).pipe(gulp.dest('dist/content/director'));

	var buildDriver = gulp.src([
		'app/content/driver/*.html'
	]).pipe(gulp.dest('dist/content/driver'));

	var buildCss = gulp.src([
		'app/css/custom.min.css',
		]).pipe(gulp.dest('dist/css'));

	var buildJs = gulp.src([
		'app/js/scripts.min.js',
		]).pipe(gulp.dest('dist/js'));

	var buildFonts = gulp.src([
		'app/fonts/**/*',
		]).pipe(gulp.dest('dist/fonts'));


});

gulp.task('deploy', function() {

	var conn = ftp.create({
		host:      'hostname.com',
		user:      'username',
		password:  'userpassword',
		parallel:  10,
		log: gutil.log
	});

	var globs = [
	'dist/**',
	'dist/.htaccess',
	];
	return gulp.src(globs, {buffer: false})
	.pipe(conn.dest('/path/to/folder/on/server'));

});

gulp.task('removedist', function() { return del.sync('dist'); });
gulp.task('clearcache', function () { return cache.clearAll(); });

gulp.task('default', ['watch']);
