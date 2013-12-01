function distribution = PlotData(varargin)

    % Required argument
    filePath = varargin{1};

    % Default optional argument values
    failThresh = 333;   % loop failure threshold in ms  
    width = 0.5;        % bin width in ms
    lineStyle = '-';
    faceColor = 'b';

    % Get optional arguments
    for i = 2:2:nargin
        if  strcmp(varargin{i}, 'Width'), width = varargin{i+1};
        elseif  strcmp(varargin{i}, 'FailThresh'), failThresh = varargin{i+1};
        elseif  strcmp(varargin{i}, 'LineStyle'), lineStyle = varargin{i+1};
        elseif  strcmp(varargin{i}, 'FaceColor'), faceColor = varargin{i+1};
        else error('Invalid argument.');
        end
    end

    % Import data
    data = csvread(filePath);
    data = data * 1000;	% convert to ms
    
    % Format data
    fails = nnz(data > failThresh);
    data = data(data < failThresh);
    data = sort(data);  % sort in assening order

    % Display summary
    [p n e] = fileparts(filePath);
    disp(sprintf(strcat('\n', n, e, ' summary:')));
    disp(sprintf('Number of samples:\t%d', numel(data)));
    disp(sprintf('Loop failures:\t\t%d', fails));
    disp(sprintf('Mean latency:\t\t%f ms', mean(data)));
    disp(sprintf('99%% less than:\t\t%f ms', data(round(length(data)*0.99))));
    disp(sprintf('Worst case:\t\t\t%f ms', data(end)));

    % Plot distribution
    edges = floor(min(data)):width:ceil(max(data));
    dist = histc(data, edges);
    bar(edges + (width / 2), dist / sum(dist), 'LineStyle', lineStyle, 'FaceColor', faceColor, 'barwidth', 1);
    xlabel('Closed-loop latency (ms)');
    ylabel('Normalised number of samples');
    title(sprintf('Distribution of %d samples', numel(data)));

    distribution = data;
end
